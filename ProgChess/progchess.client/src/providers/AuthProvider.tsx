import {
  createContext,
  useContext,
  useEffect,
  useLayoutEffect,
  useMemo,
  useState,
} from "react";
import axios from "axios";
import { api } from "../utils/api";

interface AuthContextType {
  token: string | null;
}

const AuthContext = createContext<AuthContextType>();

// Refactor
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
    const requestInterceptor = api.interceptors.request.use(
      (config) => {
        if (token) {
          api.defaults.headers["Authorization"] = `Bearer ${token}`;
        } else {
          delete api.defaults.headers["Authorization"];
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    return () => {
      api.interceptors.request.eject(requestInterceptor);
    };
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
              "http://localhost:5290/api/auth/refresh-token",
              {
                userId: localStorage.getItem("user"),
                refreshToken,
              }
            );
            const { accessToken, refreshToken: newRefreshToken } =
              response.data;
            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", newRefreshToken);
            axios.defaults.headers.common[
              "Authorization"
            ] = `Bearer ${accessToken}`;

            setToken(accessToken);

            return axios(originalRequest);
          } catch (refreshError) {
            console.error("Token refresh failed:", refreshError);
            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");
            window.location.href = "/login";
            return Promise.reject(refreshError);
          }
        }
        return Promise.reject(error);
      }
    );

    return () => {
      api.interceptors.response.eject(refreshInterceptor);
    };
  }, []);

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
