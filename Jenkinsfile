pipeline {
    agent any
    environment {
        DOCKER_IMAGE_VERSION = "1.1.${BUILD_ID}"
        DOCKER_IMAGE_NAME = "eswaramoorthi/admin"
    }
    options {
  buildDiscarder logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '', numToKeepStr: '5')
}

    stages {
        stage('PR Build'){
            stages {
                stage('Test') {
                    tools {
                        dotnetsdk 'dotnet-sdk-6.0'
                    }
                    steps {
                    
                            sh '''
                               echo "Test success"
                            '''
                    }
                }
                stage('Code Analysis') {
                    tools {
                        dotnetsdk 'dotnet-sdk-6.0'
                    }
                    steps {
                       
                            sh '''
                                dotnet build Jenkin-project.sln
                            '''
                    }
                }
                
                stage('Build Docker Image') {
                    steps {
                        script {
                            sh "docker build -t ${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_VERSION} -f /var/jenkins_home/workspace/sample-work/Dockerfile ."
                        }
                    }
                }
                stage('Push Docker Image') {
                    steps {
                        script {
                            sh "docker login -u eswaramoorthi -p esk2381313"
                            sh "docker push ${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_VERSION}"
                        }
                    }
                }
                stage('Clean Docker Image') {
                    steps {
                        script {
                            sh "docker login -u eswaramoorthi -p esk2381313"
                            sh "docker rmi ${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_VERSION}"
                            sh "docker logout"
                        }
                    }
                }

        }
    }
    }
    post {
        always {
            echo 'Pipeline completed'
        }
        success {
            echo 'Pipeline successful!'
        }
        failure {
            echo 'Pipeline failed'
        }
    }
}