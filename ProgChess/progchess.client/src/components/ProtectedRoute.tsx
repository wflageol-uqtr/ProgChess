import { useAuth } from "../providers/AuthProvider";
import ErrorPage from "../pages/error/ErrorPage";
import { Outlet } from "react-router";

export default function ProtectedRoute() {
  const { token } = useAuth();

  if (!token) {
    return <ErrorPage />;
  }

  return <Outlet />;
}
