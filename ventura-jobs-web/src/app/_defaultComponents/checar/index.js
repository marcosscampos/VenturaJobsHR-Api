import Swal from "sweetalert2";

export const checar = {
    temErroEnvioMensagem(erro, callback) {
        if (erro != null) {
            var mensagem = "";
            if (erro.errors != null) {
                // eslint-disable-next-line
                for (const [key, value] of Object.entries(erro.errors)) {
                    if (mensagem != "") {
                        mensagem = mensagem + "<br/>";
                    }
                    mensagem = mensagem + value;
                }
            }
            else {
                if (erro.status != null) {
                    mensagem = mensagem + "Erro code: " + erro.status + "<br/>"
                }
                if (erro.title != null) {
                    mensagem = mensagem + "Erro title: " + erro.title;
                }
            }
            // eslint-disable-next-line
            Swal.fire({
                icon: 'error',
                title: erro.title,
                html: mensagem,
                confirmButtonText: 'OK'
            })
            callback(true);
        } else {
            // eslint-disable-next-line
            Swal.fire({
                html: `
                  <h1 class="swal-header-h1">Sucesso!</h1>
                  <p class="swal-content-p">Mensagem disparada para o segmento!</p>
              `,
                confirmButtonText: 'Fechar'
            }).then(function(result) {
                if(result.isConfirmed) {
                    window.location.reload();
                }
            })
            callback(false);
        }
    },
}