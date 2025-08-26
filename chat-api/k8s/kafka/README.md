# Kafka deployment Guide

Note: We are deploying Kafka + Zookeeper in a Single Cluster + Single Node + No Ingress(Only with static IP as of now)
	  Deployed with Confluent Framework for both Kafka + Zookeeper
	  Security is not a main concern as of now since its a dev env to test features. Will upgrade security if made public.

STEP 1: 
----------------------------------------------
First create and deploy zookeeper.yml file
It is required to store kafka's meta data

This gives a base for kafka.

We have used v: 7.2.15(latest in confluence as of now) for both zookeeper and kafka from confluence.

We are exposing zookeeper at port: 2181(DEFAULT) 

Then Lets create a Loadbalancer service for kafka at kafka-service.yml

Will be deploying this zookeeper and kafka-service together or can be deployed independently.
Both will work as base for kafka & kafka UI(This we r using to access topic and other data via Web UI)

kafka-service will have a selector section which basically will have the name of kafka statefulset as such:
  selector:
    app: kafka


STEP 2: 
--------------------------------------------
Once we deploy above 2, from kubectl we can get the external ip address of this loadbalancer via command: 
kubectl get service kafka-service

We can note this ip address amd use this in kafka.yml as external ip address.

In kafka statefulset, we can refer zookeeper as such:
 - name: KAFKA_ZOOKEEPER_CONNECT
              value: zookeeper:2181

and external(outside cluster) and internal(inside cluster) ip addresses csn be set as such 
- name: KAFKA_ADVERTISED_LISTENERS
              value: PLAINTEXT://kafka-service:9092,EXTERNAL://<your-loadbalancer-ip-address>:9093

Above you can see 34.14.221.164 is a ip address we got from creating kafka-service loadbalancer. and 
PLAINTEXT://kafka-service:9092 is basically we r giving the name of our kafka-service itself which automcatically resolves the requests 
such as coming form kafka-ui which is deployed in same cluster 

We r creating this kafka as statefulset and not deployment because we will b having topics and other data which will b lost if created as deployment.

STEP 3: 
-------------------------------------------
Deploy kafka-ui.yml too

Since kafka-ui has its own service created in the last, so it will have its own public IP through wich we can access the website

and 
 - name: KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS
                      value: kafka-service:9092
 is used so that kafka-ui can internally call the kafka-service(the one which we depkloyed first) which resolves the request to kafka statefulset and returns back the data



 STEP 4:
 ------------------------------------------
 There is no step 4. Enjoy!. Your Kafka is ready TO use.

 IMP NOTE: Whiile deploying these through CI/CD in gitlab, i have disabled all other stages and deployed this independently.
 Because this is one time and wont require much changes often, I have disabled this Infra part from CI/CD process of .gitlab-ci.yml





