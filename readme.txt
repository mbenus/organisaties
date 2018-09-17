# build image
docker build -t glibs/organisations .

# the container will need access to the dbase from the host, so create a docker network for having the host a fixed ip
docker network create -d bridge --subnet 192.168.0.0/24 --gateway 192.168.0.1 dockernet
# 420e6cb368b976536762b41dd6143d6c200f8a92f76f2c3eb265964a89fcdbfc

# run container on port 6000
docker run -d  -p=6001:6000 glibs/organisations --net=dockernet

# try it
curl -XGET http://localhost:6001/api/organisations?sortColumn=naam
