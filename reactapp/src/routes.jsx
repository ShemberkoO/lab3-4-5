import * as React from 'react';
import Home from './Pages/Home/Home';
import Accidents from './Pages/Accidents/Accidents';
import People from './Pages/People/People';
import ShowPerson from './Pages/People/ShowPerson';
import ShowAccident from './Pages/Accidents/ShowAccident';
const routes = [
    {
        path: '/',
        element: <Home />,
    },
    {
        path: '/accidents',
        element: <Accidents />,
    },
    {
        path: '/people',
        element: <People />,
    },
    {
        path: '/people/:personId',
        element: <ShowPerson />,
    },
    {
        path: '/accidents/:accidentId',
        element: <ShowAccident />,
    },
];

export default routes;
