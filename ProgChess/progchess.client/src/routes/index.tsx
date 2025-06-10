import { createBrowserRouter, RouterProvider } from "react-router";
import ProtectedRoute from "../components/ProtectedRoute";
import StudentLogin from "../pages/auth/StudentLogin";
import AdminLogin from "../pages/auth/AdminLogin";
import CreateExercise from "../pages/exercice/CreateExercise";
import ListExercise from "../pages/exercice/ListExercise";
import Exercice from "../pages/Exercise";
import EditExercise from "../pages/exercice/EditExercise";
import ErrorPage from "../pages/error/ErrorPage";
import CookieProvider from "../providers/CookieProvider";
import Score from "../pages/Score";
import ListScore from "../pages/score/ListScore";

export default function Routes() {
  // route public accessble par non-authentifié
  const publicRoutes = [
    {
      path: "/",
      errorElement: <ErrorPage />,
      children: [
        {
          path: "/login/:id",
          element: <StudentLogin />,
        },
        {
          path: "/exercise/:id",
          element: (
            <CookieProvider>
              <Exercice />
            </CookieProvider>
          ),
        },
        {
          path: "/admin/login",
          element: <AdminLogin />,
        },
        {
          path: "/score",
          element: <Score />,
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
        {
          path: "score",
          element: <ListScore />,
        },
      ],
    },
  ];

  const router = createBrowserRouter([...publicRoutes, ...privateRoutes]);

  return <RouterProvider router={router} />;
}
