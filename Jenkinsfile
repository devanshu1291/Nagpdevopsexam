pipeline {
    agent any


stages{
  stage ('Checkout') {
  steps {
     checkout([ $class:'GitSCM', branches:[[name: '*/main']] , userRemoteConfigs:[[credentialsId: 'github', url: 'https://github.com/devanshu1291/jenkinsdevopsdemo']]])
    }
    
  }
pipeline {
    agent any
    
environment {
        username = 'devanshugoyal'
		registry = 'devanshu123/devanshugoyal'
		properties = null
		docker_port = null
    }
   
stages{
  stage ('Checkout') {
  steps {
     checkout([ $class:'GitSCM', branches:[[name: '*/main']] , userRemoteConfigs:[[credentialsId: 'github', url: 'https://github.com/devanshu1291/jenkinsdevopsdemo']]])
    }
    
  }
  stage('Restore packages') {
  steps {
    bat "dotnet restore WebApplication4\\WebApplication4.csproj"
  }
}

 stage('Build') {
 steps {
  bat 'dotnet clean WebApplication4\\WebApplication4.csproj'     
bat 'dotnet build WebApplication4\\WebApplication4.csproj -c Release -o WebApplication4/app/build'
    
 
 }
}
 stage('test') {
 steps {
bat 'dotnet test WebApplication4\\WebApplication4.csproj --logger "trx;LogFileNameWebApplication4-unit_tests.xml"'

    
 
 }
}
 
		  stage('Build Docker Image') {
      steps {
             bat "dotnet publish WebApplication4\\WebApplication4.csproj"
             
            bat "docker build -t i-${username}-master --no-cache -f Dockerfile ."
        
      }
    }
     
     stage('Move image to DockerHub') {
      steps {
             bat "docker tag i-${username}-master ${registry}:${BUILD_NUMBER}"
             withDockerRegistry([credentialsId:'DockerHub', url:""]){
             bat "docker push ${registry}:${BUILD_NUMBER}"
             }
        
      }
    }
     stage('Docker Deployment') {
      steps {
             bat "docker run --name c-${username}-master -d -p 7100:80 ${registry}:${BUILD_NUMBER}"
            
      }
    }

}
}