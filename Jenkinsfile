pipeline {

    agent {
        node {
            label 'servidor-webservice-produccion'
        }
    }

    environment {
        VERSION_DESPLIEGUE  = '1.0.5'
        VERSION_PRODUCCION  = '1.0.4'
        NOMBRE_CONTENEDOR   = 'cnt-ws-transacciones-spi'
        NOMBRE_IMAGEN       = 'img_ws_transacciones_spi'
        PUERTO              = '9031'
        PUERTO_CONTENEDOR   = '8080'
        RUTA_CONFIG         = '/config/wsTransaccionesSPI/'
        RUTA_PLANTILLAS     = '/plantillas/spi/'
        RUTA_LOGOS          = '/logos/'
    }

    stages {

        stage('Build') {
            steps {
                echo 'Building ...'
                sh 'docker build -t ${NOMBRE_IMAGEN}:${VERSION_DESPLIEGUE} --no-cache .'
            }
        }

        stage('Test') {
            steps {
                echo 'Testing ...'
            }
        }

        stage('Clean') {
            steps {
                echo 'Cleaning ...'
                sh 'docker rm -f ${NOMBRE_CONTENEDOR}'
            }
        }

        stage('Deploy') {
            steps {
                echo 'Deploying ...'
                sh  '''docker run --restart=always -it -dp ${PUERTO}:${PUERTO_CONTENEDOR} --name ${NOMBRE_CONTENEDOR} \
                        -e TZ=${TZ} \
                        -v ${RUTA_CONFIG}appsettings.json:/app/appsettings.json \
                        -v ${RUTA_PLANTILLAS}:/app/Plantillas \
                        -v ${RUTA_LOGOS}:/app/Logos \
                        ${NOMBRE_IMAGEN}:${VERSION_DESPLIEGUE}
                    '''
            }
        }
        stage('Restart') {
            steps {
                echo 'Restarting ...'
                sh 'docker restart ${NOMBRE_CONTENEDOR}'
            }
        }
    }

    post {

        success {
            slackSend color: '#BADA55', message: "Despliegue exitoso - ${env.JOB_NAME} version publicada ${VERSION_DESPLIEGUE}  (<${env.BUILD_URL}|Open>)"
        }

        failure {
            sh  'docker rm -f ${NOMBRE_CONTENEDOR}'
            sh  '''docker run --restart=always -it -dp ${PUERTO}:${PUERTO_CONTENEDOR} --name ${NOMBRE_CONTENEDOR} \
                        -e TZ=${TZ} \
                        -v ${RUTA_CONFIG}appsettings.json:/app/appsettings.json \
                        -v ${RUTA_PLANTILLAS}:/app/Plantillas \
                        -v ${RUTA_LOGOS}:/app/Logos \
                        ${NOMBRE_IMAGEN}:${VERSION_PRODUCCION}
                    '''
            slackSend color: '#FE2D00', failOnError:true, message:"Despliegue fallido 😬 - ${env.JOB_NAME} he reversado a la version ${VERSION_PRODUCCION}  (<${env.BUILD_URL}|Open>)"
        }
    }
}

