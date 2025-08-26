terraform {
backend "http" {
    address        = "https://gitlab.com/api/v4/projects/73360675/terraform/state/kafka"
    lock_address   = ""
    unlock_address = ""
    username       = "gitlab-ci-token"
  }
  required_providers {
    kafka = {
      source  = "Mongey/kafka"
      version = "0.5.3"
    }
  }
}
variable "gitlab_token" {
  description = "GitLab CI Job Token passed from pipeline"
}

provider "kafka" {
  bootstrap_servers = [
    "34.14.221.164:9093"
  ]
  tls_enabled = false
  sasl_username = ""
  sasl_password = ""
}