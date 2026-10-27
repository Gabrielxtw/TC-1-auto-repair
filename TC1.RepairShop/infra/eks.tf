data "aws_caller_identity" "current" {}

locals {
  # Learner Lab does not allow creating IAM roles, so the cluster and the nodes use the
  # pre-created LabRole. Building its ARN from the account ID avoids an IAM lookup.
  lab_role_arn = "arn:aws:iam::${data.aws_caller_identity.current.account_id}:role/LabRole"
}

resource "aws_eks_cluster" "this" {
  name     = var.cluster_name
  version  = var.kubernetes_version
  role_arn = local.lab_role_arn

  vpc_config {
    subnet_ids = aws_subnet.public[*].id
  }

  access_config {
    authentication_mode = "API"
    # Whoever runs Terraform (the Learner Lab voclabs role, locally or in CI) becomes cluster admin.
    bootstrap_cluster_creator_admin_permissions = true
  }

  # Auto-upgrade when standard support ends instead of moving to the pricier extended support.
  upgrade_policy {
    support_type = "STANDARD"
  }
}

resource "aws_eks_node_group" "this" {
  cluster_name    = aws_eks_cluster.this.name
  node_group_name = "default"
  node_role_arn   = local.lab_role_arn
  subnet_ids      = aws_subnet.public[*].id
  ami_type        = "AL2023_x86_64_STANDARD"
  instance_types  = [var.node_instance_type]

  scaling_config {
    min_size     = var.node_min_size
    desired_size = var.node_desired_size
    max_size     = var.node_max_size
  }

  # Nodes need the internet route to join the cluster and pull images.
  depends_on = [aws_route_table_association.public]
}

# Required by the Horizontal Pod Autoscaler to read pod CPU and memory usage.
# vpc-cni, kube-proxy and coredns are installed by EKS by default.
resource "aws_eks_addon" "metrics_server" {
  cluster_name = aws_eks_cluster.this.name
  addon_name   = "metrics-server"

  # The add-on only becomes ACTIVE once there are nodes to run it.
  depends_on = [aws_eks_node_group.this]
}
