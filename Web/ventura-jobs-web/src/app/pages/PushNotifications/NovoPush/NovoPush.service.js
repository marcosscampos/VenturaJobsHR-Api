import httpClient from "@/core/services/httpClient";

const url = process.env.VUE_APP_ROOT_API + '/v1/mensagem';

class NovoPushService {
    constructor() { }

    async EnviarMensagem(mensagem) {
        return new Promise((resolve, reject) => {
            httpClient.post(url, mensagem).then(response => {
                if(response.data.statusCode == 500) {
                    reject(response.data)
                } else {
                    resolve(response.data)
                }
            }).catch((error) => {
                reject(error.response.data)
            })
        })
    }
}

export default new NovoPushService();