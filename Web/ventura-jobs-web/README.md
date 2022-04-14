# blue-template-vue

### Setup do projeto

```
npm install
```

---

### Compilar e rodar localmente

```
npm run serve
```

Você pode usar a URL gerada pelo build para começar as configurações básicas de rota e layout do projeto.

---

### Estrutura do projeto

- src
  - app
    - \_defaultComponents
    - pages
  - assets
  - core

Não é recomendável alterar nenhum arquivo das pastas `core` e `assets`. Essas pastas serão utilizadas para atualizar o template no futuro. Caso seja necessário alterar algum arquivo dessas pastas para seu projeto, entre em contato com a equipe de arquitetura.

A pasta `app` é onde seu projeto deve ser desenvolvido. Os arquivos das pastas `_defaultComponents` precisam permanecer no projeto. Caso não seja necessário usar eles no seu projeto, é possível comentar todo o conteúdo desses arquivos.

O arquivo `router.js` e `main.js` estão na pasta `src`.

---

### Compilar para produção

```
npm run build
```
