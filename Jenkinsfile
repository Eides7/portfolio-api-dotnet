pipeline {
    agent any

    triggers {
    	pollSCM('H/2 * * * *')
    }
    
    options {
        timestamps()
    }
  
    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --no-restore -c Release'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test --no-build -c Release'
            }
        }
    }
}
