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
import ListScore from "../pages/score/ListScore";
import ForgotPassword from "../pages/auth/ForgotPassword";
import ResetPassword from "../pages/auth/ResetPassword";
import CreateScore from "../pages/score/CreateScore";
import EditScore from "../pages/score/EditScore";
import ListStudent from "../pages/student/ListStudent";
import EditStudent from "../pages/student/EditStudent";
import ListImage from "../pages/image/ListImage";

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
          path: "/admin/forgot-password",
          element: <ForgotPassword />,
        },
        {
          path: "/admin/reset-password",
          element: <ResetPassword />,
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
        {
          path: "score/create",
          element: <CreateScore />,
        },
        {
          path: "score/edit/:id",
          element: <EditScore />,
        },
        {
          path: "students",
          element: <ListStudent />,
        },
        {
          path: "student/edit/:id",
          element: <EditStudent />,
        },
        {
          path: "image",
          element: <ListImage />,
        },
      ],
    },
  ];

  const router = createBrowserRouter([...publicRoutes, ...privateRoutes]);

  return <RouterProvider router={router} />;
}
