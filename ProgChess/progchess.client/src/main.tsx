import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router";
import "./index.css";
import AdminLogin from "./pages/auth/AdminLogin.tsx";
import Dashboard from "./pages/Dashboard.tsx";
import NotFound from "./pages/error/NotFound.tsx";
import StudentLogin from "./pages/auth/StudentLogin.tsx";

const router = createBrowserRouter([
  { path: "/login", Component: StudentLogin },
  { path: "/admin/login", Component: AdminLogin },
  { path: "/", Component: Dashboard, errorElement: <NotFound /> },
]);

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
