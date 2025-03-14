import { createBrowserRouter } from "react-router";
import App from "../components/App";
import HomePage from "../pages/HomePage";
import AboutPage from "../pages/AboutPage";
import ContactPage from "../pages/ContactPage";
import CatalogPage from "../pages/catalog/CatalogPage";
import { Typography } from "@mui/material";
import ProductDetailsPage from "../pages/catalog/ProductDetailsPage";
import ErrorPage from "../pages/ErrorPage";
import ServerError from "../errors/ServerError";
import NotFound from "../errors/NotFound";

export const router = createBrowserRouter(
    [
        {
            path: "/",
            element: <App />,
            children: [
                { path: "", element: <HomePage /> },
                { path: "about", element: <AboutPage /> },
                { path: "contact", element: <ContactPage /> },
                { path: "catalog", element: <CatalogPage /> },
                { path: "catalog/:id", element: <ProductDetailsPage /> },
                { path: "error-test", element: <ErrorPage /> },
                { path: "server-error", element: <ServerError /> },
                { path: "not-found", element: <NotFound /> },
                { path: "*", element: <Typography margin={5}>Page Not Found</Typography> },
            ]
        }
    ]
)