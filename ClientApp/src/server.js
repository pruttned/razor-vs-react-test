import App from './App';
import React from 'react';
import { renderToString } from 'react-dom/server';
import { StaticRouter } from 'react-router-dom';
import 'isomorphic-fetch';
import routes from './routes';
import { renderRoutes, matchRoutes } from 'react-router-config';
import { ServerStyleSheet } from 'styled-components';

const assets = require(process.env.RAZZLE_ASSETS_MANIFEST);

const server = (url, baseUrl, model) => {
    const matchingRoutes = matchRoutes(routes, url);
    const currentRoute = matchingRoutes[0]
    const loadDataUrl = currentRoute.route.loadData.replace('{id}', currentRoute.match.params && currentRoute.match.params.id); //TODO: make it general

    var dataFetch = model ? Promise.resolve(model) : fetch(baseUrl + loadDataUrl).then(resp => resp.json());

    return dataFetch
        .then(data => {
            const sheet = new ServerStyleSheet();
            const context = { data };
            const markup = renderToString(
                sheet.collectStyles(
                    <StaticRouter context={context} location={url}>
                        <App />
                    </StaticRouter>)
            );
            const styleTags = sheet.getStyleTags();
            return `<!doctype html>
                    <html lang="">
                    <head>
                        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
                        <meta charset="utf-8" />
                        <title>Welcome to Razzle</title>
                        <meta name="viewport" content="width=device-width, initial-scale=1">
                        <script>window.__ROUTE_DATA__=${JSON.stringify(data)}</script>
                        ${
                assets.client.css
                    ? `<link rel="stylesheet" href="${assets.client.css}">`
                    : ''
                }
                        ${
                process.env.NODE_ENV === 'production'
                    ? `<script src="${assets.client.js}" defer></script>`
                    : `<script src="${assets.client.js}" defer crossorigin></script>`
                }
                ${styleTags}
                    </head>
                    <body>
                        <div id="root">${markup}</div>
                    </body>
                </html>`;
        });
}
export default server;