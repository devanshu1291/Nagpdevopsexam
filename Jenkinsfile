pipeline {
    agent any
    
    environment {
        scannerHome = tool name: 'sonar_scanner_dotnet'
         username = 'devanshugoyal'
		registry = 'devanshu123/devanshugoyal'
        docker_port = "${env.BRANCH_NAME == "master" ? "7200" : "7300"}"
	CONTAINER_ID = null
	project_id = 'testjenkinsapi-321513'
       cluster_name = 'dotnet-api'
       location = 'us-central1-c'
       credentials_id = 'TestJenkinsApi'
    }
    
   

    
    options {
        timestamps()
        timeout(time: 1, unit: 'HOURS')
        buildDiscarder logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '10', numToKeepStr: '20')
    }
    
    stages {
		//stage ('Checkout') {
 // steps {
     //checkout([ $class:'GitSCM', branches:[[name: '*/develop']] , userRemoteConfigs:[[credentialsId: 'github', url: 'https://github.com/devanshu1291/finaldevopsassigment']]])
  //echo "Start Checkout"
//  }
    
//  }
		stage('nuget restore') {
			steps {
				echo "Start restoring packages"
				bat "dotnet restore WebApplication4\\WebApplication4.csproj"
			}
		}
        
      stage('Start Sonarqube Analysis') {
			  when {
				  expression {
					  BRANCH_NAME == 'master'
				  }
			 }
        steps {
            echo "Start Sonarqube analysis"
               withSonarQubeEnv('Test_Sonar') {
                               bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:WebApplication4 /n:WebApplication4 /v:1.0"

              }
            }
        }
     
        stage('Code build') {
            steps {
                echo "Clean"
                bat 'dotnet clean WebApplication4\\WebApplication4.csproj'   
				
		echo "Start Building"
		bat 'dotnet build WebApplication4\\WebApplication4.csproj -c Release -o WebApplication4/app/build'
            }
        }
		
		stage('Stop Sonarqube Analysis') {
			when {
				expression {
					BRANCH_NAME == 'master'
				}
			}
            steps {
                echo "Stop Sonarqube analysis"
               withSonarQubeEnv('Test_Sonar'){
            bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
        }
            }
        }
		
		stage('Release artifact') {
			when {
				expression {
					BRANCH_NAME == 'develop'
				}
			}
			steps {
				echo "Publish Code"
				bat "dotnet publish -c Release"
			}
		}
		
		stage('Docker Image') {
			steps {
				echo "Create Docker Image"
				bat "docker build -t i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
				bat "docker tag i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} ${registry}:${BUILD_NUMBER}"
				bat "docker tag i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} ${registry}:latest"
			}
		}
		
		stage('Containers') {
			parallel {
				stage('PreContainerCheck') {
					environment {
				        CONTAINER_ID = "${bat(script:"docker ps -a --filter publish=${docker_port} --format {{.ID}}", returnStdout: true).trim().readLines().drop(1).join("")}" 
				    }
				    steps {
                        echo "Running pre container check"
				        script {
				        if(env.CONTAINER_ID != null) {
				            echo "Removing container: ${env.CONTAINER_ID}"
				            bat "docker rm -f ${env.CONTAINER_ID}"        
				        }
				   }
                                   
                    }
				}
				stage('PushtoDockerHub') {
					steps {
						echo "Push Image to Docker Hub"
				
						withDockerRegistry([credentialsId: 'DockerHub', url: ""]) {
							bat "docker push ${registry}:${BUILD_NUMBER}"
							bat "docker push ${registry}:latest"
						}
					}
				}
			}
		}
		
		stage('Docker Deployment') {
			steps {
				echo "Docker Deployment started"
				bat "docker run --name c-${username}-${BRANCH_NAME} -d -p ${docker_port}:80 ${registry}:${BUILD_NUMBER}"
			}
		}
	    
	    stage('Kubernetes Deployment') {
			steps {
				echo "Deploying to Kubernetes"
		    
   bat "kubectl apply -f deployment.yaml"
				
				
			}
		}
		
		
	}

        
}
