var server = require('./ClientApp/build/server');
module.exports = async function (url, baseUrl, model) {
    return await server(url, baseUrl, model);
};
