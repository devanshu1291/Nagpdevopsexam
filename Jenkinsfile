pipeline {
    agent any
    environment{
        scannerHome = tool name: 'sonar_scanner_dotnet'
        username = 'devanshugoyal'
		registry = 'devanshu123/devanshugoyal'
    }
    stages {
         stage ('Checkout') {
  steps {
     checkout([ $class:'GitSCM', branches:[[name: '*/feature']] , userRemoteConfigs:[[credentialsId: 'github', url: 'https://github.com/devanshu1291/Nagpdevopsexam.git']]])
    }
    }
      stage('Restore packages') {
  steps {
    bat "dotnet restore WebApplication4\\WebApplication4.csproj"
  }
}

stage('Start Sonar Analysis') {
      steps {
        withSonarQubeEnv('Test_Sonar'){
            bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:WebApplication4 /n:WebApplication4 /v:1.0"
        }
        
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
stage('End Sonar Analysis') {
      steps {
        withSonarQubeEnv('Test_Sonar'){
            bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
        }
        
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
     stage('Deployment') {
         parallel{
       stage('Docker Deployment') {
      steps {
             bat "docker run --name c-${username}-master -d -p 7400:80 ${registry}:${BUILD_NUMBER}"
            
      }
    }
       stage('Kubernetes Deployment') {
			steps {
				echo "Deploying to Kubernetes"
			

bat " gcloud auth activate-service-account --key-file=key.json"
bat "gcloud container clusters get-credentials cluster-1 --zone us-central1-c --project idyllic-now-321512"
bat "kubectl apply -f deployment.yaml"


  
				
				
			}
		}
}}}}