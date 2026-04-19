"use client";

import React, { useState } from "react";
import {
  Box,
  Drawer,
  AppBar,
  Toolbar,
  IconButton,
  Typography,
  ListSubheader,
  Avatar,
  Badge,
  CssBaseline,
  ThemeProvider,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Collapse,
} from "@mui/material";
import { LuMenu } from "react-icons/lu";
import { HiOutlineBell } from "react-icons/hi";
import {
  MdExpandLess,
  MdExpandMore,
  MdShowChart,
  MdNotes,
} from "react-icons/md";
import { useRouter, usePathname } from "next/navigation";
import { theme } from "../_shared/theme";

const drawerWidth = 256;

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const [mobileOpen, setMobileOpen] = useState(false);
  const [strategiesOpen, setStrategiesOpen] = useState(true);

  const router = useRouter();
  const pathname = usePathname();

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const drawerContent = (
    <Box sx={{ overflow: "auto" }}>
      <List disablePadding>
        {/* Strategies Section */}
        <ListSubheader>Strategies</ListSubheader>
        <ListItemButton onClick={() => setStrategiesOpen(!strategiesOpen)}>
          <ListItemIcon>
            <MdShowChart size={20} />
          </ListItemIcon>
          <ListItemText primary="Backtesting" />
          {strategiesOpen ? <MdExpandLess /> : <MdExpandMore />}
        </ListItemButton>
        <Collapse in={strategiesOpen} timeout="auto" unmountOnExit>
          <List component="div" disablePadding>
            <ListItemButton
              sx={{ pl: 4 }}
              selected={pathname === "/backtesting/moving-average"}
              onClick={() => router.push("/backtesting/moving-average")}
            >
              <ListItemText primary="Moving Average" />
            </ListItemButton>
          </List>
        </Collapse>

        {/* Notes Section */}
        <ListSubheader>Other</ListSubheader>
        <ListItemButton
          selected={pathname === "/notes"}
          onClick={() => router.push("/notes")}
        >
          <ListItemIcon>
            <MdNotes size={20} />
          </ListItemIcon>
          <ListItemText primary="Notes" />
        </ListItemButton>
      </List>
    </Box>
  );

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Box sx={{ display: "flex" }}>
        <AppBar>
          <Toolbar>
            <IconButton
              color="inherit"
              onClick={handleDrawerToggle}
              sx={{ mr: 2, display: { lg: "none" } }}
            >
              <LuMenu />
            </IconButton>

            <Typography
              variant="h6"
              noWrap
              sx={{ flexGrow: 1, fontWeight: "bold" }}
            >
              Dashboard.UI
            </Typography>

            <Box sx={{ display: "flex", gap: 1, alignItems: "center" }}>
              <IconButton color="inherit">
                <Badge variant="dot" color="primary">
                  <HiOutlineBell size={30} />
                </Badge>
              </IconButton>
              <Avatar
                sx={{
                  width: 32,
                  height: 32,
                  bgcolor: "neutral.main",
                  fontSize: "0.875rem",
                }}
              >
                DT
              </Avatar>
            </Box>
          </Toolbar>
        </AppBar>

        {/* --- SIDEBAR (DRAWER) --- */}
        <Box
          component="nav"
          sx={{ width: { lg: drawerWidth }, flexShrink: { lg: 0 } }}
        >
          {/* --- Mobile View --- */}
          <Drawer
            variant="temporary"
            open={mobileOpen}
            onClose={handleDrawerToggle}
            ModalProps={{ keepMounted: true }}
          >
            <Toolbar />
            {drawerContent}
          </Drawer>

          {/* --- Desktop View --- */}
          <Drawer variant="permanent">
            <Toolbar />
            {drawerContent}
          </Drawer>
        </Box>

        {/* --- MAIN CONTENT --- */}
        <Box
          component="main"
          sx={{ flexGrow: 1, p: 3, bgcolor: "grey.50", minHeight: "100vh" }}
        >
          <Toolbar />
          {children}
        </Box>
      </Box>
    </ThemeProvider>
  );
}
