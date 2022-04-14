// const webpack = require("webpack");
// const path = require("path");
//
// module.exports = {
//   publicPath: process.env.VUE_APP_BASE_URL,
//   filenameHashing: false,
//   configureWebpack: {
//
//     mode: process.env.NODE_ENV.toLowerCase(),
//     output: {
//       libraryTarget: 'umd'
//     },
//     devServer: {
//       headers: { 'Access-Control-Allow-Origin': '*' }
//     },
//     plugins: [
//       new webpack.optimize.LimitChunkCountPlugin({
//         maxChunks: 1
//       })
//     ],
//     resolve: {
//       alias: {
//         // If using the runtime only build
//         vue$: "vue/dist/vue.runtime.esm.js" // 'vue/dist/vue.runtime.common.js' for webpack 1
//         // Or if using full build of Vue (runtime + compiler)
//         // vue$: 'vue/dist/vue.esm.js'      // 'vue/dist/vue.common.js' for webpack 1
//       }
//     }
//   },
//   chainWebpack: config => {
//     config.module
//       .rule("eslint")
//       .use("eslint-loader")
//       .tap(options => {
//         options.configFile = path.resolve(__dirname, ".eslintrc.js");
//         return options;
//       });
//   },
//   css: {
//     extract: false,
//     loaderOptions: {
//       postcss: {
//         config: {
//           path: __dirname
//         }
//       },
//       scss: {
//         prependData: `@import "@/assets/sass/vendors/vue/vuetify/variables.scss";`
//       }
//     }
//   },
//   transpileDependencies: ["vuetify"]
// };
