﻿const path = require("path");
const webpack = require('webpack');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

//Https설정 불러오기
const HttpsConfigGet = require('./AspNetCore_HttpsConfigGet');


//소스 위치
const RootPath = path.resolve(__dirname);
const SrcPath = path.resolve(RootPath, 'src');

module.exports = (env, argv) => 
{
    console.log("*** RootPath  = " + RootPath);
    console.log("    SrcPath  = " + SrcPath);

    //릴리즈(프로덕션)인지 여부
    const EnvPrductionIs = false;
    const EnvString = "development";
    if (argv.mode === "production")
    {
        EnvPrductionIs = true;
        EnvString = "production";
    }
    console.log("*** Mode  = " + argv.mode);

    return {
        /** 서비스 모드 */
        mode: EnvString,
        devtool: "inline-source-map",

        entry: "./src/index.ts",
        output: {
            //path: path.resolve(__dirname, "build", argv.mode),
            path: path.resolve(__dirname, "build", EnvString),
            //filename: "[name].[chunkhash].js",
            filename: "app.js",
            publicPath: "/",
        },

        resolve: {
            extensions: [".js", ".ts"],
            alias: {
                '@': SrcPath,
            },
        },
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    use: "ts-loader",
                },
                {
                    test: /\.css$/,
                    use: [MiniCssExtractPlugin.loader, "css-loader"],
                },
            ],
        },
        plugins: [
            new webpack.SourceMapDevToolPlugin({}),
            new CleanWebpackPlugin(),
            new HtmlWebpackPlugin({
                template: "./src/index.html",
            }),
            new MiniCssExtractPlugin({
                filename: "css/[name].[chunkhash].css",
            }),
        ],

        devServer: {
            /** 서비스 포트 */
            port: "9601",
            https: HttpsConfigGet(true),
            proxy: {
                "/api/":
                {
                    target: "https://localhost:7250",
                    logLevel: "debug",
                    //호스트 헤더 변경 허용
                    changeOrigin: true,
                    secure: false,
                    onProxyReq: function (proxyReq, req, res)
                    {
                        console.log(`[HPM] [${req.method}] ${req.url}`);
                        //console.log(" ~~~~ proxyReq ~~~~");
                        //console.log(proxyReq);
                        //console.log(" ~~~~ res ~~~~");
                        //console.log(res);
                    },
                },
            },

            /** 출력파일의 위치 */
            static: [path.resolve("./", "build", "development/")],
            /** 브라우저 열지 여부 */
            open: false,
            /** 핫리로드 사용여부 */
            hot: true,
            /** 라이브 리로드 사용여부 */
            liveReload: true
        },
    };
    
}