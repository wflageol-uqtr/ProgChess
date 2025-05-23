import { createBrowserRouter, RouterProvider } from "react-router";
import ProtectedRoute from "../components/ProtectedRoute";
import StudentLogin from "../pages/auth/StudentLogin";
import AdminLogin from "../pages/auth/AdminLogin";
import CreateExercice from "../pages/exercice/CreateExercice";
import ListExercice from "../pages/exercice/ListExercice";
import Exercice from "../pages/Exercice";
import EditExercice from "../pages/exercice/EditExercice";

export default function Routes() {
  // route public accessble par non-authentifié
  const publicRoutes = [
    {
      path: "/login",
      element: <StudentLogin />,
    },
    {
      path: "/exercice/:id",
      element: <Exercice />,
    },
    {
      path: "/admin/login",
      element: <AdminLogin />,
    },
  ];

  // route accessible par admin
  const privateRoutes = [
    {
      path: "/admin",
      element: <ProtectedRoute />,
      children: [
        {
          path: "exercice",
          element: <ListExercice />,
        },
        {
          path: "exercice/create",
          element: <CreateExercice />,
        },
        {
          path: "exercice/edit/:id",
          element: <EditExercice />,
        },
      ],
    },
  ];

  const router = createBrowserRouter([...publicRoutes, ...privateRoutes]);

  return <RouterProvider router={router} />;
}
