import { createBrowserRouter, RouterProvider } from "react-router";
import ProtectedRoute from "../components/ProtectedRoute";
import StudentLogin from "../pages/auth/StudentLogin";
import { useAuth } from "../providers/AuthProvider";
import AdminLogin from "../pages/auth/AdminLogin";
import Dashboard from "../pages/Dashboard";

export default function Routes() {
  const { token } = useAuth();

  // route public accessble par non-authentifié
  const routesForNotAuthenticatedOnly = [
    {
      path: "/login",
      element: <StudentLogin />,
    },
    {
      path: "/admin/login",
      element: <AdminLogin />,
    },
  ];

  // route accessible par admin
  const routesForAuthenticatedOnly = [
    {
      path: "/",
      element: <ProtectedRoute />,
      children: [
        {
          path: "/dashboard",
          element: <Dashboard />,
        },
        {
          path: "/logout",
          element: <div>Logout</div>,
        },
      ],
    },
  ];

  const router = createBrowserRouter([
    ...(!token ? routesForNotAuthenticatedOnly : []),
    ...routesForAuthenticatedOnly,
  ]);

  return <RouterProvider router={router} />;
}
