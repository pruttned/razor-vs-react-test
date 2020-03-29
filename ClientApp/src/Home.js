import React, { useState, useEffect } from 'react';
import logo from './react.svg';
import './Home.css';
import Layout from './Layout';
import { withRouter, Link } from 'react-router-dom';
import 'isomorphic-fetch';
import styled from 'styled-components';

const NewsItemRoot = styled.div`
  border: solid 1px silver;
  padding: 5px;
  max-width: 600px;
  margin-bottom: 10px;
`;

const NewsItem = ({ alias, title, excerpt }) => (
  <NewsItemRoot>
    <div>
      <Link to={`/Home/DetailReact/${alias}`}>{title}</Link>
    </div>
    <div>
      {excerpt}
    </div>
  </NewsItemRoot>
);

const Home = (props) => {
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
      {data ?
        <div>
          {data.news.map(nn => (<NewsItem key={nn.alias} {...nn} ></NewsItem>))}
        </div> :
        <div>loading..</div>
      }
    </Layout>
  );
};

export default withRouter(Home);
