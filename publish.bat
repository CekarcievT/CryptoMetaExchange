docker stop BSDigitalContainer
docker rm BSDigitalContainer
docker build -t cryptoexchangeimage:v1 -f Dockerfile . --no-cache
docker run -it -p 5000:5000 --name BSDigitalContainer cryptoexchangeimage:v1