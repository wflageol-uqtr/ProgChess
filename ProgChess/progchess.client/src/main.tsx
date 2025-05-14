import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router";
import "./index.css";
import Login from "./pages/auth/Login.tsx";
import Dashboard from "./pages/Dashboard.tsx";
import NotFound from "./pages/error/NotFound.tsx";

const router = createBrowserRouter([
  { path: "/login", Component: Login },
  { path: "/", Component: Dashboard, errorElement: <NotFound /> },
]);

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
