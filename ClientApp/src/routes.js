import Home from "./Home";
import Detail from "./Detail";

const routes = [
    {
        path: "/Home/IndexReact",
        component: Home,
        exact: true,
        loadData: '/Home/GetAll',
    },
    {
        path: "/Home/DetailReact/:id",
        component: Detail,
        exact: true,
        loadData: '/Home/GetDetail?alias={id}',
    },
];

export default routes;