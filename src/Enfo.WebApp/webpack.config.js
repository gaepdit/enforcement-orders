const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    entry: {
        site: './Client/site.js',
        datepicker: {
            dependOn: 'site',
            import: './Client/datepicker.js'
        },
        formSearch: {
            dependOn: 'site',
            import: './Client/formSearch.js'
        },
        formAddOrder: {
            dependOn: 'site',
            import: './Client/formAddOrder.js'
        },
        formEditOrder: {
            dependOn: 'site',
            import: './Client/formEditOrder.js'
        },
        formValidation: {
            dependOn: 'site',
            import: './Client/formValidation.js'
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
            {
                test: /\.(png|jpe?g|gif)$/i,
                type: 'asset/resource'
            },
            {
                test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
                type: 'asset/resource'
            },
            {
                test: /\.(woff2?)$/i,
                type: 'asset'
            },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/i,
                type: 'asset'
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/i,
                type: 'asset/resource'
            }
        ]
    }
}