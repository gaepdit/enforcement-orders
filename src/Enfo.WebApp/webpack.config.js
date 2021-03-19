﻿const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    entry: {
        main: {
            dependOn: 'vendor',
            import: './Client/main.js'
        },
        vendor: './Client/vendor.js',
        vendorTimepicker: {
            dependOn: 'vendor',
            import: './Client/vendor-timepicker.js'
        },
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot', 'dist'),
        clean: true
    },
    devtool: 'source-map',
    plugins: [new MiniCssExtractPlugin()],
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: [MiniCssExtractPlugin.loader, 'css-loader']
            },
            {test: /\.(png|jpe?g|gif)$/i, loader: 'file-loader'},
            {test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: 'file-loader'},
            {
                test: /\.(woff|woff2)$/i,
                loader: 'url-loader',
                options: {prefix: 'font', limit: 5000}
            },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/i,
                loader: 'url-loader',
                options: {limit: 10000, 'mimetype': 'application/octet-stream'}
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/i,
                loader: 'file-loader'
            }
        ]
    }
}