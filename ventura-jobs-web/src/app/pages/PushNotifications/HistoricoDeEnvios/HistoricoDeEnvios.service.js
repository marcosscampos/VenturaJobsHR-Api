import httpClient from "@/core/services/httpClient";

const url = process.env.VUE_APP_ROOT_API + '/v1/mensagem';

class HistoricoDeEnviosService {
    constructor() { }

    async obterMensagensEnviadas() {
        return new Promise((callback) => {
            httpClient.get(url).then(response => {
                callback(response.data)
            })
        })
    }
}

export default new HistoricoDeEnviosService();