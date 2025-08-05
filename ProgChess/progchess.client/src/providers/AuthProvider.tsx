import {
  createContext,
  useContext,
  useEffect,
  useLayoutEffect,
  useMemo,
  useState,
} from "react";
import axios from "axios";
import api, { apiUrl } from "../utils/api";
import { handleApiError } from "../utils/apiErrorHandler";
interface AuthContextType {
  token: string | null;
  setToken: React.Dispatch<React.SetStateAction<string | null>>;
}

const AuthContext = createContext<AuthContextType>();

const AuthProvider = ({ children }: any) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("accessToken")
  );

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      setToken(accessToken);
    }
  }, []);

  useEffect(() => {
    const verifyToken = async () => {
      try {
        await api.get("/api/auth/verify-token");
        setToken(localStorage.getItem("accessToken"));
      } catch (error) {
        setToken(null);
      }
    };

    if (token) {
      verifyToken();
    }
  }, []);

  useLayoutEffect(() => {
    const refreshInterceptor = api.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true;
          try {
            const refreshToken = localStorage.getItem("refreshToken");
            const response = await axios.post(
              `${apiUrl}/api/auth/refresh-token`,
              {
                userId: localStorage.getItem("user"),
                refreshToken,
              }
            );
            const { accessToken, refreshToken: newRefreshToken } =
              response.data;
            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", newRefreshToken);

            originalRequest.headers.Authorization = `Bearer ${accessToken}`;
            setToken(accessToken);

            return axios(originalRequest);
          } catch (refreshError) {
            handleApiError(error);
            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");
            localStorage.removeItem("user");
            return Promise.reject({ redirectTo: "/admin/login" });
          }
        }
        return Promise.reject({ error });
      }
    );

    return () => {
      api.interceptors.response.eject(refreshInterceptor);
    };
  }, []);

  // En parler avec William
  const contextValue = useMemo(
    () => ({
      token,
      setToken,
    }),
    [token]
  );

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export default AuthProvider;
