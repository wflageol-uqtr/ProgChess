import { createBrowserRouter, RouterProvider } from "react-router";
import ProtectedRoute from "../components/ProtectedRoute";
import StudentLogin from "../pages/auth/StudentLogin";
import AdminLogin from "../pages/auth/AdminLogin";
import CreateExercise from "../pages/exercice/CreateExercise";
import ListExercise from "../pages/exercice/ListExercise";
import Exercice from "../pages/Exercise";
import EditExercise from "../pages/exercice/EditExercise";
import ErrorPage from "../pages/error/ErrorPage";

export default function Routes() {
  // route public accessble par non-authentifié
  const publicRoutes = [
    {
      path: "/",
      errorElement: <ErrorPage />,
      children: [
        {
          path: "/login",
          element: <StudentLogin />,
        },
        {
          path: "/exercise/:id",
          element: <Exercice />,
        },
        {
          path: "/admin/login",
          element: <AdminLogin />,
        },
      ],
    },
  ];

  // route accessible par admin
  const privateRoutes = [
    {
      path: "/admin",
      element: <ProtectedRoute />,
      children: [
        {
          path: "exercise",
          element: <ListExercise />,
        },
        {
          path: "exercise/create",
          element: <CreateExercise />,
        },
        {
          path: "exercise/edit/:id",
          element: <EditExercise />,
        },
      ],
    },
  ];

  const router = createBrowserRouter([...publicRoutes, ...privateRoutes]);

  return <RouterProvider router={router} />;
}
