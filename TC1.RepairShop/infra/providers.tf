terraform {
  # 1.11+ is required for S3 native state locking (use_lockfile).
  required_version = "~> 1.11"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.69"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.9"
    }
  }

  # The bucket name includes the AWS account ID, so it is passed at init time:
  #   terraform init -backend-config="bucket=tc1-repairshop-tfstate-<account-id>"
  backend "s3" {
    key          = "tc1-repairshop/terraform.tfstate"
    region       = "us-east-1"
    encrypt      = true
    use_lockfile = true
  }
}

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project   = "tc1-repairshop"
      ManagedBy = "terraform"
    }
  }
}
