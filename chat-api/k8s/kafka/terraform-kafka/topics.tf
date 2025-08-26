resource "kafka_topic" "topics" {
  for_each = { for t in var.kafka_topics : t.name => t }

  name               = each.value.name
  partitions         = each.value.partitions
  replication_factor = each.value.replication_factor

  config = {
    "cleanup.policy" = "delete"
  }
}
