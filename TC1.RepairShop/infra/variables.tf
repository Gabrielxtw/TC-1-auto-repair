variable "aws_region" {
  description = "AWS Region. AWS Academy Learner Lab only allows us-east-1 and us-west-2."
  type        = string
  default     = "us-east-1"

  validation {
    condition     = contains(["us-east-1", "us-west-2"], var.aws_region)
    error_message = "AWS Academy Learner Lab only allows the us-east-1 and us-west-2 Regions."
  }
}

variable "availability_zone_ids" {
  description = "Availability Zone IDs of the subnets. IDs are the same in every account; names such as us-east-1a are not."
  type        = list(string)
  default     = ["use1-az1", "use1-az2"]

  validation {
    condition     = length(var.availability_zone_ids) >= 2 && length(setintersection(var.availability_zone_ids, ["use1-az3", "usw1-az2", "cac1-az3"])) == 0
    error_message = "EKS needs at least two zones and does not support use1-az3, usw1-az2 or cac1-az3."
  }
}

variable "cluster_name" {
  description = "EKS cluster name, also used as the prefix of the other resources."
  type        = string
  default     = "tc1-repairshop"
}

variable "kubernetes_version" {
  description = "EKS Kubernetes version. Keep it in standard support to avoid extended support charges."
  type        = string
  default     = "1.36"
}

variable "node_instance_type" {
  description = "EC2 instance type of the EKS nodes. t3.small (2 GiB, 11 pods per node) is the smallest that leaves room for the API next to the system pods; t3.micro allows only 4 pods."
  type        = string
  default     = "t3.small"
}

variable "node_min_size" {
  description = "Minimum number of EKS nodes."
  type        = number
  default     = 1
}

variable "node_desired_size" {
  description = "Number of EKS nodes created initially."
  type        = number
  default     = 2
}

variable "node_max_size" {
  description = "Maximum number of EKS nodes."
  type        = number
  default     = 3
}

variable "db_instance_type" {
  description = "EC2 instance type of the SQL Server host. SQL Server needs at least 2 GB of memory."
  type        = string
  default     = "t3.medium"
}

variable "db_image" {
  description = "SQL Server container image, pinned to a cumulative update."
  type        = string
  default     = "mcr.microsoft.com/mssql/server:2022-CU27-GDR1-ubuntu-22.04"
}

variable "db_name" {
  description = "Database name used in the connection string; the API creates it on startup (EF Core migrations)."
  type        = string
  default     = "TC1RepairShop"
}
