import moment from 'moment';
import {mapState} from "vuex";

import {SET_BREADCRUMB} from "@/core/store/breadcrumbs.module";
import {OBTER_CLIENTES} from "@/core/store/NotificationPush/BaseDeClientes";

export default {
    components: {},
    computed: mapState({
        clientes: state => state.baseDeClientes.clientes
    }),
    data() {
        return {
            filter: '',
            isBusy: false,
            dataConsulta: new Date(),
            fields: [
                {
                    key: 'login',
                    label: 'Login'
                },
                {
                    key: 'nome',
                    label: 'Nome'
                },
                {
                    key: 'quantidadeDeDispositivos',
                    label: 'Dispositivos',
                },
                {
                    key: 'segmento',
                    label: 'Segmentos',
                },
                {
                    key: 'tipoNotificacao',
                    label: 'Research'
                },
                {
                    key: 'tipoNotificacaoOfertas',
                    label: 'Ofertas'
                },
                {
                    key: 'tipoNotificacaoMovimentacoes',
                    label: 'Movimentações'
                },
                {
                    key: 'tipoNotificacaoFavoritos',
                    label: 'Favoritos'
                }
            ],
            options: [
                {value: "Geral", text: "Geral"}
            ],
            selected: 'Geral'
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
        this.$store.dispatch(SET_BREADCRUMB, [
            {title: "Push Notifications", route: '/'},
            {title: "Base de Clientes", route: "base-clientes"}
        ]);
    },
    mounted() {
        this.isBusy = true
        this.buscarCliente()

        this.unsub = this.$store.subscribe((mutation, state) => {
            if (mutation.type == 'baseDeClientes/' + OBTER_CLIENTES) {
                this.isBusy = false;
            }
        })
    },
    methods: {
        buscarCliente() {
            this.dataConsulta = new Date();
            this.$store.dispatch({type: 'baseDeClientes/obterClientes', filter: this.filter})
        },
        limparPesquisa() {
            this.dataConsulta = new Date();
            this.filter = '';
            this.buscarCliente();
        }
    },
    beforeDestroy() {
        this.unsub();
    }
}