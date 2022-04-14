import httpClient from "@/core/services/httpClient"

const url = process.env.VUE_APP_ROOT_API + '/v1/clientes';

class BaseDeClientesService {
    constructor() { }

    async obterClientes(filter) {
        return new Promise((callback) => {
            httpClient.get(url + `?Nome=${filter}`).then(response => {
                callback(response.data)
            })
        })
    }
}

export default new BaseDeClientesService();