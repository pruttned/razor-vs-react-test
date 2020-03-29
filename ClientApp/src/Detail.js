import React, { useState, useEffect } from 'react';
import logo from './react.svg';
import './Home.css';
import Layout from './Layout';
import { withRouter } from 'react-router-dom';
import 'isomorphic-fetch';
import styled from 'styled-components';

const Root = styled.div`
  padding: 5px;
  max-width: 600px;
  margin-bottom: 100px;

  h1{
    color: silver;
  }
`;

const Detail = (props) => {
  let initData = null;
  if (props.staticContext) {
    initData = props.staticContext.data || {};
  } else if (window.__ROUTE_DATA__) {
    initData = window.__ROUTE_DATA__;
    window.__ROUTE_DATA__ = undefined;
  }

  const [data, setData] = useState(initData);

  if (!data) {
    const loadDataUrl = props.route.loadData.replace('{id}', props.match.params && props.match.params.id); //TODO: make it general
    fetch(loadDataUrl)
      .then(resp => resp.json())
      .then(d => setData(d));
  }

  return (
    <Layout>
      <Root>
        {data ?
          <>
            <h1>
              {data.title}
            </h1>
            <div>
              <div dangerouslySetInnerHTML={{ __html: data.text }} />
            </div></> :
          <div>loading..</div>
        }
      </Root>
    </Layout>
  );
}

export default withRouter(Detail);
