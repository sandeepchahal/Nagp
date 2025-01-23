#!/bin/bash

# Apply MongoDB files
kubectl apply -f mongodb/mongodb-statefulset.yaml
kubectl apply -f mongodb/mongodb-service.yaml

# Apply SQL Server files
kubectl apply -f sql-server/sql-server-secret.yaml
kubectl apply -f sql-server/sql-server-statefulset.yaml
kubectl apply -f sql-server/sql-server-service.yaml

# Apply Elasticsearch files

kubectl apply -f elasticsearch/elasticsearch-statefulset.yaml
kubectl apply -f elasticsearch/elasticsearch-service.yaml


echo "All database resources applied successfully!"
