output "cluster_name" {
  description = "EKS cluster name."
  value       = aws_eks_cluster.this.name
}

output "configure_kubectl" {
  description = "Command that adds the cluster to the local kubeconfig."
  value       = "aws eks update-kubeconfig --region ${var.aws_region} --name ${aws_eks_cluster.this.name}"
}

output "db_host" {
  description = "Private IP of the SQL Server host (reachable only from the cluster)."
  value       = aws_instance.db.private_ip
}

output "db_instance_id" {
  description = "SQL Server host instance ID, for SSM Session Manager."
  value       = aws_instance.db.id
}

output "db_password" {
  description = "SQL Server sa password. Read with: terraform output -raw db_password"
  value       = random_password.db.result
  sensitive   = true
}

output "db_connection_string" {
  description = "Value for the ConnectionStrings__DefaultConnection Kubernetes Secret."
  value       = "Server=${aws_instance.db.private_ip},1433;Database=${var.db_name};User Id=sa;Password=${random_password.db.result};TrustServerCertificate=True"
  sensitive   = true
}
