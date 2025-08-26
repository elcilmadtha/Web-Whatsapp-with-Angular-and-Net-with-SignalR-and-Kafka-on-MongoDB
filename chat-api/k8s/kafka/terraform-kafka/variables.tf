variable "kafka_topics" {
  type = list(object({
    name              = string
    partitions        = number
    replication_factor = number
  }))
}