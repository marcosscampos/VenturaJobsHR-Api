import {SET_BREADCRUMB} from "@/core/store/breadcrumbs.module";
import moment from 'moment';

import NovoPush from '../NovoPush/index'
import {OBTER_MENSAGENS} from "@/core/store/NotificationPush/HistoricoDeEnvios";
import {mapState} from "vuex";

export default {
    components: {NovoPush},
    computed: mapState({
        mensagens: state => state.historicoDeEnvios.mensagens
    }),
    data() {
        return {
            filter: '',
            dataConsulta: new Date(),
            fields: [
                {
                    key: 'dataEnvio',
                    label: 'Data de Envio'
                },
                {
                    key: 'titulo',
                    label: 'Titulo'
                },
                {
                    key: 'resumo',
                    label: 'Resumo'
                },
                {
                    key: 'mensagem',
                    label: 'Mensagem'
                },
                {
                    key: 'segmento',
                    label: 'Segmento'
                },
                {
                    key: 'tipo',
                    label: 'Tipo'
                },
                {
                    key: 'quantidadeClientes',
                    label: 'Clientes'
                },
                {
                    key: 'quantidadeMensagensEnviadas',
                    label: 'Enviados'
                },
                {
                    key: 'quantidadeMensagensLidas',
                    label: 'Lidos'
                }
            ],
            isBusy: false,
        }
    },
    filters: {
        moment: (date) => {
            if (date != null) {
                return moment(date).format("DD/MM/YYYY HH:mm:ss")
            } else {
                return "";
            }
        }
    },
    beforeMount() {
        this.$store.dispatch("historicoDeEnvios/obterMensagensEnviadas")
        this.$store.dispatch(SET_BREADCRUMB, [
            {title: "Push Notifications", route: '/'},
            {title: "Histórico de Envios", route: "historico-envios"}
        ]);
    },
    mounted() {
        this.isBusy = true;
        this.dataConsulta = new Date();

        this.unsub = this.$store.subscribe((mutation, state) => {
            if (mutation.type == 'historicoDeEnvios/' + OBTER_MENSAGENS) {
                this.isBusy = false;
            }
        })
    },
    methods: {
        fechaModal() {
            this.$refs['modal'].hide()
        }
    },
    beforeDestroy() {
        this.unsub();
    }
}