<template>
  <div id="push-modal">
    <h1>Novo Envio de Push</h1>
    <div class="box-segmentos">
      <h1 class="box-h1">Segmento</h1>
      <b-form-select v-model="mensagem.segmento" class="mb-3">
        <option v-for="(option, key) in options" :key="key" :value="option.value" v-text="option.text"></option>
      </b-form-select>
    </div>
    <div class="box-tipo">
      <h1 class="box-h1">Tipos</h1>
      <b-form-select v-model="mensagem.tipo" class="mb-3">
        <option v-for="(option, key) in optionsTipo" :key="key" :value="option.value" v-text="option.text"></option>
      </b-form-select>
    </div>
    <div class="box-titulo">
      <h1 class="box-h1">Título</h1>
      <b-form-input v-model="mensagem.titulo" id="input-titulo" maxlength="40">
      </b-form-input>
      <div id="count-titulo" class="contador">{{ countTitulo }}/40</div>
    </div>
    <div class="box-resumo">
      <h1 class="box-h1">Resumo (Enviado no push)</h1>
      <b-form-textarea
          v-model="mensagem.resumo"
          id="textarea-resumo"
          rows="3"
          max-rows="3"
          maxlength="100"
          no-resize></b-form-textarea>
      <div id="count-resumo" class="contador">{{ count }}/100</div>
    </div>
    <div class="box-mensagem">
      <h1 class="box-h1">Mensagem (Exibido no App)</h1>
      <vue-editor v-model="mensagem.conteudo" :editor-toolbar="customToolbar">
      </vue-editor>
    </div>
    <div class="box-botaoAcao">
      <h1 class="box-h1">Url do botão de ação (Exibido no App)</h1>
      <b-form-input v-model="mensagem.urlBotao"></b-form-input>
    </div>
    <div class="box-titulo_botaoAcao">
      <h1 class="box-h1">Título do botão de ação</h1>
      <b-form-input v-model="mensagem.tituloBotao"></b-form-input>
    </div>
    <div style="display: flex; flex-direction: row; justify-content: space-between; margin-top: 30px">
      <b-button @click="fecharModal" style="width: 45%; height: 50px">Voltar</b-button>
      <b-button @click="enviarPush" variant="primary" style="width: 45%;  height: 50px">ENVIAR PUSH</b-button>
    </div>
  </div>
</template>

<script>
import {VueEditor} from 'vue2-editor'
import {mapState} from "vuex";

import {ENVIAR_MENSAGEM} from "@/core/store/NotificationPush/NovoPush";
import { checar } from '../../../_defaultComponents/checar/index'

export default {
  name: "NovoPush",
  components: {VueEditor},
  computed: mapState({
    erro: state => state.novoPush.erro
  }),
  props: {
    fecharModal: Function
  },
  data: () => {
    return {
      count: 0,
      countTitulo: 0,
      options: [
        {value: "Geral", text: "Geral"}
      ],
      selectedTipo: 'Todos',
      optionsTipo: [
        {value: 'Todos', text: 'Todos'}
      ],
      mensagem: {},
      customToolbar: [
        [{'header': [1, 2, 3, 4, 5, 6, false]}],
        ["bold"], ["italic"], ["underline"], ["link"],
        [{list: "ordered"}], [{list: "bullet"}], ["clean"]
      ]
    }
  },
  methods: {
    enviarPush() {
      this.$store.dispatch({type: 'novoPush/enviarMensagem', mensagem: this.mensagem})
    },
    validacaoLarguraMaxima() {
      const that = this;

      document.getElementById("textarea-resumo").onkeyup = function () {
        that.count = this.value.length;
      }

      document.getElementById("input-titulo").onkeyup = function () {
        that.countTitulo = this.value.length;
      }
    }
  },
  mounted() {
    this.validacaoLarguraMaxima();

    this.unsub = this.$store.subscribe((mutation, state) => {
      if(mutation.type == 'novoPush/' + ENVIAR_MENSAGEM) {
        checar.temErroEnvioMensagem(this.erro, (result) => {
          if(!result) {  }
        })
      }
    })
  },
  beforeDestroy() {
    this.unsub();
  }
}
</script>

<style>
div#push-modal h1 {
  color: #3F4254;
  font-size: 16px;
  font-family: 'Poppins', serif;
  margin-bottom: 20px;
}

div#push-modal p {
  font-size: 13px;
  color: #495057
}

div#push-modal .box-h1 {
  font-size: 13px;
  color: #646C9B;
  margin-bottom: 8px;
}

div#push-modal .box-segmentos,
div#push-modal .box-tipo,
div#push-modal .box-resumo,
div#push-modal .box-titulo,
div#push-modal .box-mensagem,
div#push-modal .box-botaoAcao,
div#push-modal .box-titulo_botaoAcao {
  padding: 0;
  margin-top: 20px
}

div#push-modal .box-titulo {
  margin-top: 5px
}

div.swal2-popup.swal2-modal.swal2-show {
  max-width: 300px;
  max-height: 145px;
  padding: 20px;
}

div.swal2-icon-error {
  max-width: none !important;
  max-height: none !important;
  padding: 0;
}

.swal-header-h1 {
  font-size: 18px;
  color: #3F4254;
  text-align: left;
}

.swal-content-p {
  font-size: 13px;
  text-align: left;
  margin-top: 20px;
  margin-bottom: 0;
}

button.swal2-confirm.swal2-styled {
  font-size: 13px;
  color: #0072CE;
  background: transparent;
}

button.swal2-confirm:hover,
button.swal2-styled:hover {
  background: transparent !important;
}

div.swal2-actions {
  justify-content: flex-end !important;
  margin-top: 0 !important;
}

.contador {
  font-size: 11px;
  text-align: right;
  margin-top: 5px;
  color: #646C9B
}

div.ql-toolbar.ql-snow {
  display: flex;
  flex-direction: row;
  justify-content: center;
  flex-wrap: wrap;
}
</style>