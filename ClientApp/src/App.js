import React from 'react';
import Route from 'react-router-dom/Route';
import Switch from 'react-router-dom/Switch';
import { renderRoutes, matchRoutes } from 'react-router-config';
import routes from './routes';

import './App.css';

const App = () => {
  if (typeof window === 'undefined') {
    global.window = {}
  }

  return (<Switch>
    {renderRoutes(routes)}
  </Switch>
  )
};

export default App;
