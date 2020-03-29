import React from 'react';
import logo from './react.svg';
import './Home.css';
import Detail from './Detail';
import { Link } from 'react-router-dom';

const Layout = (props) => (
    <>
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <a className="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">RazorApp</a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <Link to={`/Home/IndexReact`}>Home</Link>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        </header>
        <div className="container">
            <main role="main" className="pb-3">
                {props.children}
            </main>
        </div>

        <footer className="border-top footer text-muted">
            <div className="container">
                &copy; 2020 - RazorApp - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
            </div>
        </footer>
    </>
)

export default Layout;
