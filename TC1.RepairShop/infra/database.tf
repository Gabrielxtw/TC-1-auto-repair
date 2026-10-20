# Learner Lab's RDS does not offer SQL Server, so SQL Server 2022 Express runs as the official
# container on an EC2 instance.

# Alphanumeric: satisfies the SQL Server password policy and never needs escaping.
resource "random_password" "db" {
  length      = 32
  special     = false
  min_upper   = 2
  min_lower   = 2
  min_numeric = 2
}

data "aws_ami" "al2023" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["al2023-ami-2023.*-x86_64"]
  }
}

resource "aws_security_group" "db" {
  name        = "${var.cluster_name}-sqlserver"
  description = "SQL Server reachable only from the EKS cluster"
  vpc_id      = aws_vpc.this.id

  ingress {
    description     = "SQL Server from EKS nodes and pods"
    from_port       = 1433
    to_port         = 1433
    protocol        = "tcp"
    security_groups = [aws_eks_cluster.this.vpc_config[0].cluster_security_group_id]
  }

  egress {
    description = "HTTPS for OS packages, the container image and SSM"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.cluster_name}-sqlserver"
  }
}

resource "aws_instance" "db" {
  ami                    = data.aws_ami.al2023.id
  instance_type          = var.db_instance_type
  subnet_id              = aws_subnet.public[0].id
  vpc_security_group_ids = [aws_security_group.db.id]
  ebs_optimized          = true
  # Pre-created by Learner Lab. Its LabRole includes AmazonSSMManagedInstanceCore, so
  # Session Manager gives a shell on the host without SSH or an open port.
  iam_instance_profile = "LabInstanceProfile"

  user_data_replace_on_change = true
  user_data                   = <<-EOT
    #!/bin/bash
    set -euo pipefail
    dnf install -y docker
    systemctl enable --now docker
    docker run --detach --name mssql --restart unless-stopped \
      --env ACCEPT_EULA=Y --env MSSQL_PID=Express \
      --env MSSQL_SA_PASSWORD='${random_password.db.result}' \
      --publish 1433:1433 --volume mssql-data:/var/opt/mssql \
      '${var.db_image}'
  EOT

  root_block_device {
    volume_type = "gp3"
    volume_size = 20
    encrypted   = true
  }

  metadata_options {
    http_tokens = "required"
  }

  tags = {
    Name = "${var.cluster_name}-sqlserver"
  }

  lifecycle {
    # The data lives on this instance: a newer Amazon Linux AMI must not recreate it (and wipe the database).
    ignore_changes = [ami]
  }

  # The startup script downloads Docker and the SQL Server image on first boot.
  depends_on = [aws_route_table_association.public]
}
