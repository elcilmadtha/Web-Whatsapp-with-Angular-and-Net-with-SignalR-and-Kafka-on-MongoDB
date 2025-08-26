# .NET DEPLOYMENT GUIDE

We r deploying in Google Cloud Platform(GCP)


Create Service Manager Account in google console
Enable Artifact Registry API 
Enable Google Kubernetes Engine API

Now for the Service Manager Account give necesary access to deploy and read and write

Get variabled such as cluster name, zone, project id, secret key json file etc from gcp and save it in gitlab > settings > ci/cd > variables section

That's all. You are ready to Deploy.

Detailed guide for kafka deployment I have put inside k8s folder. check it out.


------------------------------


INGRESS GUIDE 
-------------
Intially we had installed services without a domain and only with IP's.
Now that we have a subdomain handy, lets configure it.

We need to install a 1 time ingress controller which will work as a entry point and will talk to both current loadbalancers of angular and .net
So since ingress talks to loadbalancers from inside the cluster we can safely convert them to cluster IP.

1st step is run 
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/deploy.yaml   on GCP CONSOLE
and validate - kubectl get pods -n ingress-nginx

we will get a External IP address. Put that in the godaddy domain DNS > A record .
Same IP for both chat.<mydomain>.in & api.<mydomain>.in and make it as a A record.

Convert current services in Angular and .Net from loadbalancer to ClusterIP 

Add this new chat.<mydomain>.in (UI domain) in .Net to allow CORS

Deploy both angular and .net.

Once deployed then run infra job(one time) for ingress.yml

This should run process with subdomains in the entire flow.

NEXT STEP: SSL from LetsEncrypt
-------------------------------
Install 1 time
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.15.0/cert-manager.yaml

check if pods are ready - kubectl get pods -n cert-manager

Then deploy 
kubectl apply -f cluster-issuer.yml

