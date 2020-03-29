var server = require('./ClientApp/build/server');

async function run() {
    const res1 = await server('/Home/DetailReact/news2', 'http://localhost:5000');
    console.log(res1);
};
run().catch(e => console.log(e));

console.log();
// console.log('----------------------');
// console.log(server('/detail/news1'));
