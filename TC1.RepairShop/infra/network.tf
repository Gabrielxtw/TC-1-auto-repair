resource "aws_vpc" "this" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_support   = true
  enable_dns_hostnames = true

  tags = {
    Name = var.cluster_name
  }
}

# Strips every rule from the VPC default security group so nothing uses it by accident.
resource "aws_default_security_group" "this" {
  vpc_id = aws_vpc.this.id
}

resource "aws_internet_gateway" "this" {
  vpc_id = aws_vpc.this.id

  tags = {
    Name = var.cluster_name
  }
}

# Public subnets only: nodes and the database host reach the internet through the internet
# gateway, which avoids paying for a NAT gateway. Security groups still block inbound traffic.
resource "aws_subnet" "public" {
  count = length(var.availability_zone_ids)

  vpc_id               = aws_vpc.this.id
  availability_zone_id = var.availability_zone_ids[count.index]
  cidr_block           = cidrsubnet(aws_vpc.this.cidr_block, 4, count.index)

  # EKS managed nodes in public subnets need a public IP to join the cluster and pull images.
  map_public_ip_on_launch = true

  tags = {
    Name                     = "${var.cluster_name}-public-${var.availability_zone_ids[count.index]}"
    "kubernetes.io/role/elb" = "1"
  }
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.this.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.this.id
  }

  tags = {
    Name = "${var.cluster_name}-public"
  }
}

resource "aws_route_table_association" "public" {
  count = length(aws_subnet.public)

  subnet_id      = aws_subnet.public[count.index].id
  route_table_id = aws_route_table.public.id
}
