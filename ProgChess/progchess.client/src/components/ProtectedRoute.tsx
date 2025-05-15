import { useAuth } from "../providers/AuthProvider";
import NotFound from "../pages/error/NotFound";
import { Outlet } from "react-router";

export default function ProtectedRoute() {
  const { token } = useAuth();

  if (!token) {
    return <NotFound />;
  }

  return <Outlet />;
}
