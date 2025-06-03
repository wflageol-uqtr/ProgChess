import { LogOut } from "lucide-react";
import { useNavigate } from "react-router";

export default function Logout() {
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
      navigate("/admin/login");
    } catch (error) {
      throw Error("Une erreur est arriv. lors de la deconnexion");
    }
  };

  return (
    <a
      onClick={handleLogout}
      className="flex space-x-2 hover:bg-gray-100 rounded text-white hover:text-gray-900 py-2"
      href="#"
    >
      <LogOut className="text-red-600" />
      <span>Déconnexion</span>
    </a>
  );
}
